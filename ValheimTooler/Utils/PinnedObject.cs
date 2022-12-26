using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ValheimTooler.Utils
{
    internal class PinnedObject : MonoBehaviour
    {
        public void Init(string aName)
        {
            this.pin = Minimap.instance.AddPin(base.transform.position, Minimap.PinType.Icon3, aName, false, false);
            Debug.Log(string.Format("Tracking: {0} at {1} {2} {3}", new object[]
            {
                aName,
                base.transform.position.x,
                base.transform.position.y,
                base.transform.position.z
            }));
        }

        private void OnDestroy()
        {
            bool flag = this.pin != null && Minimap.instance != null;
            if (flag)
            {
                Minimap.instance.RemovePin(this.pin);
                Debug.Log(string.Format("Removing: {0} at {1} {2} {3}", new object[]
                {
                    this.pin.m_name,
                    base.transform.position.x,
                    base.transform.position.y,
                    base.transform.position.z
                }));
            }
        }

        // Token: 0x04000002 RID: 2
        public Minimap.PinData pin;
    }
}
