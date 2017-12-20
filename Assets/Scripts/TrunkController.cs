using UnityEngine;

namespace Assets.Scripts
{
    class TrunkController : MonoBehaviour
    {
        private bool overflowedRow;
        private Vector3 fastSpeed, slowSpeed, currentSpeed;
        private bool justSpawned;
        private bool incomingFromLeft;
        public Vector3 FastSpeed
        {
            get
            {
                return fastSpeed;
            }

            set
            {
                fastSpeed = value;
            }
        }

        public bool JustSpawned
        {
            get
            {
                return justSpawned;
            }

            set
            {
                justSpawned = value;
            }
        }

        public bool IncomingFromLeft
        {
            get
            {
                return incomingFromLeft;
            }

            set
            {
                incomingFromLeft = value;
            }
        }

        public Vector3 SlowSpeed
        {
            get
            {
                return slowSpeed;
            }

            set
            {
                slowSpeed = value;
            }
        }

        void Update()
        {
            currentSpeed = getCorrectSpeed();
            transform.Translate(currentSpeed * Time.deltaTime);
            checkRowOverflow();
        }

        private Vector3 getCorrectSpeed()
        {
            float lateralWidth = gameObject.GetComponent<Renderer>().bounds.extents.x;
            if (transform.position.x + lateralWidth < Row.rightmostBorder &&
                transform.position.x - lateralWidth > Row.leftmostBorder)
                return slowSpeed;
            return fastSpeed;
        }

        private void checkRowOverflow()
        {
            if (justSpawned)
            {
                if (incomingFromLeft && transform.position.x > Row.leftmostBorder)
                    justSpawned = false;
                else if (!incomingFromLeft && transform.position.x < Row.rightmostBorder)
                    justSpawned = false;
            }
            else
            {
                float offset = gameObject.GetComponent<Renderer>().bounds.extents.x;
                if ((incomingFromLeft && transform.position.x >
                    Row.rightmostBorder + Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x + offset) ||
                    (!incomingFromLeft && transform.position.x <
                    Row.leftmostBorder - Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x - offset))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
