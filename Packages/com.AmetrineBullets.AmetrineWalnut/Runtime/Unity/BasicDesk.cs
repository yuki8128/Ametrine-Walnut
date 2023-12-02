using System.Collections;
using System.Collections.Generic;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using UnityEngine;

namespace com.AmetrineBullets.AmetrineWalnut.Unity
{
    public class BasicDesk : Desk
    {
        public BasicDesk(IBook defaultBook)
        {
            base.defaultBook = defaultBook;
        }
    }
}