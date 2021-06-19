﻿using System;

namespace TauCode.Lab.Utility
{
    public class FileCheck : FileSystemObjectCheck
    {
        public FileCheck(string localPath, bool isMandatory, Func<string, byte[]> contentGetter)
            : base(FileSystemObjectType.File, localPath, isMandatory)
        {
            this.ContentGetter = contentGetter;
        }

        public FileCheck(string localPath, bool isMandatory, string content)
            : base(FileSystemObjectType.File, localPath, isMandatory)
        {
            this.ContentGetter = s => throw new NotImplementedException(content); // todo
        }

        public FileCheck(string localPath, bool isMandatory)
            : this(localPath, isMandatory, (Func<string, byte[]>)null)
        {
            
        }


        public Func<string, byte[]> ContentGetter { get; }
    }
}