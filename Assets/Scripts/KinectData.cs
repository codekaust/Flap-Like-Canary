using UnityEngine;
using System.Linq;
using Windows.Kinect;
using System.Collections.Generic;

public class KinectData : MonoBehaviour{

    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;

    private static float leftWristY;
    private static float rightWristY;
    private static float neckY;
    private static float hipCentreY;
	private static float elbowRightY;
    private static bool playerFound;
    public bool IsAvailable;
    private Body[] _Data = null;
 
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();

    public KinectData()
    {
        leftWristY = 0;
        rightWristY = 0;
        neckY = 0;
        hipCentreY = 0;
        playerFound = false;
    }

    public Body[] GetData()
    {
        return _Data;
    }

    public void Start() {
        _Sensor = KinectSensor.GetDefault();        
        if (_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
    }
	
	void Update () {        
        if (_Reader != null)
        {   
            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)
            {
                if (_Data == null)
                {                    
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }
                frame.GetAndRefreshBodyData(_Data);                
                frame.Dispose();
                frame = null;
            }
        }


        if (_Data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();

        foreach (var body in _Data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                print("body.TrackingId : " + body.TrackingId);
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);


        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }
        
        foreach (var body in _Data.Where(b => b.IsTracked))
        {
			if (body != null) {
				if (!_Bodies.ContainsKey (body.TrackingId)) {
					_Bodies [body.TrackingId] = new GameObject ("body: " + body.TrackingId);
				}

				if (body.Joints [JointType.SpineMid].Position.Z < 1.5) {
					playerFound = true;

					Windows.Kinect.Joint leftWrist = body.Joints [JointType.WristLeft];
					Windows.Kinect.Joint rightWrist = body.Joints [JointType.WristRight];
					Windows.Kinect.Joint hipCentre = body.Joints [JointType.SpineMid];
					Windows.Kinect.Joint neck = body.Joints [JointType.SpineShoulder];
					Windows.Kinect.Joint elbowRight = body.Joints [JointType.ElbowRight];

					leftWristY = leftWrist.Position.Y;
					rightWristY = rightWrist.Position.Y;
					hipCentreY = hipCentre.Position.Y;
					neckY = neck.Position.Y;
					elbowRightY = elbowRight.Position.Y;
					print ("leftwrist : " + leftWrist.Position.X + ", " + leftWrist.Position.Y + ", " + leftWrist.Position.Z);
					print ("rightWrist : " + rightWrist.Position.X + ", " + rightWrist.Position.Y + ", " + rightWrist.Position.Z);
				} else {
					playerFound = false;
				}
			} else if (body == null) {
				playerFound = false;
			}
        }        
    }

    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
       playerFound = false;
    }

    public float GetLeftHandY()
    {
        return leftWristY;
    }

    public float GetRightHandY()
    {
        return rightWristY;
    }

    public float GetHeadY()
    {
        return neckY;
    }

    public float GetHipY()
    {
        return hipCentreY;
    }

	public float GetElbowRight(){
		return elbowRightY;
	}

    public bool IsPlayerFound()
    {
        return playerFound;
    }
    //
}
