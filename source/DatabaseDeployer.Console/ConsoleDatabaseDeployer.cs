using System;
using System.Collections.Generic;
using DatabaseDeployer.Core.Model;
using DatabaseDeployer.Core.Services;
using DatabaseDeployer.Core.Services.Impl;

namespace DatabaseDeployer.Console
{
    public class ConsoleDatabaseDeployer:ITaskObserver
    {
        IDictionary<string,string> _properties = new Dictionary<string, string>();
        public void Log(string message)
        {
            System.Console.WriteLine(message);
        }

        public void SetVariable(string name, string value)
        {
            if(_properties.ContainsKey(name))
            {
                _properties[name] = value;
            }
            else
            {
                _properties.Add(name, value);    
            }
            
        }

        public bool UpdateDatabase(ConnectionSettings settings, string scriptDirectory, RequestedDatabaseAction action)
        {
            var manager = new SqlDatabaseManager();
            
            var taskAttributes = new TaskAttributes(settings, scriptDirectory)
                                     {
                                         RequestedDatabaseAction = action,
                                     };
            try
            {
                manager.Upgrade(taskAttributes, this);

                foreach (var property in _properties)
                {
                    Log(property.Key +": " + property.Value);
                }
                return true;
            }
            catch (Exception exception)
            {                
                var ex = exception;
                do
                {
                    Log("Failure: " + ex.Message);
                    ex = ex.InnerException;    
                } while (ex!=null);

            }
            return false;
        }
    }
}