/*
 * Created by SharpDevelop.
 * User: lexli
 * Date: 2009-1-1
 * Time: 16:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Specialized;

namespace System.Configuration
{
    /// <summary>
    /// Description of ConfigurationManager.
    /// </summary>
    public static class ConfigurationManager
    {
        public static NameValueCollection AppSettings { 
            get { return new NameValueCollection(0); }
        }
    }
}
