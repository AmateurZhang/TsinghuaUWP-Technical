using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Services
{
    public class MessageException : Exception
    {
        public new string Message;
        public MessageException(string _message)
        {
            Message = _message;
        }
    }

    public class NumberException : Exception
    {
        public new int Message;
        public NumberException(int _message)
        {
            Message = _message;
        }
    }


}
