﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public class Exceptions : Exception
    {
        public Exceptions() : base() { }
        public Exceptions(string message) : base(message) { }
        public Exceptions(string message, Exception innerException) : base(message, innerException) { }
        protected Exceptions(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public override string ToString() => base.ToString();
    }

    [Serializable]
    public class IdNotExistException : Exception
    {
        public IdNotExistException() : base() { }
        public IdNotExistException(string message) : base(message) { }
        public IdNotExistException(string message, Exception inner) : base(message, inner) { }
        protected IdNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    [Serializable]
    public class IdExistException : Exception
    {
        public IdExistException() : base() { }
        public IdExistException(string message) : base(message) { }
        public IdExistException(string message, Exception inner) : base(message, inner) { }
        protected IdExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class DalConfigException : Exception
    {
        public DalConfigException(string msg) : base(msg) { }
        public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
    }

    public class LoadingException : Exception
    {
        string? filePath;
        public LoadingException() : base() { }
        public LoadingException(string message) : base(message) { }
        public LoadingException(string message, Exception inner) : base(message, inner) { }

        public LoadingException(string path, string messege, Exception inner) => filePath = path;
        protected LoadingException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}


