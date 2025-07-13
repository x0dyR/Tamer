using UnityEngine;

namespace Tamer.Develop.Features.SpringFeature
{
    [RequireComponent(typeof(SpringJoint))]
    public class SpringConnection : MonoBehaviour
    {
        private Transform _parent;

        private SpringJoint _joint;

        private void Awake()
        {
            _joint = GetComponent<SpringJoint>();
        }

        public void Connect(Rigidbody parent)
        {
            _joint.connectedBody = parent;
        }
    }
}