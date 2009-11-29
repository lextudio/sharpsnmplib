/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/29/2009
 * Time: 11:01 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
	/// <summary>
	/// SET message handler.
	/// </summary>
	internal class SetMessageHandler : IMessageHandler
	{
	    private readonly ISnmpMessage _message;
	    private readonly ObjectStore _store;
	    private ErrorCode _status;
	    private int _index;
	    
		public SetMessageHandler(ISnmpMessage message, ObjectStore store)
		{
		    _message = message;
		    _store = store;
		}

		public IList<Variable> Handle()
		{
			_index = 0;
		    _status = ErrorCode.NoError;

			IList<Variable> result = new List<Variable>();
			foreach (Variable v in _message.Pdu.Variables) 
			{
				_index++;
				ISnmpObject obj = _store.GetObject(v.Id);
                if (obj != null)
                {
                    try
                    {
                        obj.Data = v.Data;
                    }
                    catch (AccessFailureException)
                    {
                        _status = ErrorCode.NoSuchName;
                    }
                    catch (ArgumentException)
                    {
                        _status = ErrorCode.BadValue;
                    }
                    catch (Exception)
                    {
                        _status = ErrorCode.GenError;
                    }
                }
                else
                {
                    _status = ErrorCode.NoSuchName;
                }
			    
				if (_status != ErrorCode.NoError)
				{
					return null;
				}

                result.Add(v);
			}
			
			return result;
		}
	    
        public ErrorCode ErrorStatus
        {
            get { return _status; }
        }
	    
        public int ErrorIndex 
        {
            get { return _index; }
        }
	}
}
