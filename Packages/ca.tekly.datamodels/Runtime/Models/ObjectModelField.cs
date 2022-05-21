using System;

namespace Tekly.DataModels.Models
{
    public class ObjectModelField : Attribute
    {
        public string Id;

        public ObjectModelField() { }

        public ObjectModelField(string id)
        {
            Id = id;
        }
    }
}