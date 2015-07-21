#region license

/* The MIT License (MIT)

 * Copyright (c) 2015 Skoth

 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion

using System;
using UnityEngine;

namespace KSPCommSys
{
    public enum LogType
    {
        Default,
        Error,
        Exception,
        Warning
    }

    public static class CommSysLog
    {
        private static string Tag = "[i~ {Communications Engineering} ~i]: ";

        public static void Log(string msg, LogType lt = LogType.Default) {
            // this code definitely needs some reworking :O
            string taggedMsg = Tag + msg;
            switch (lt)
            {
                case LogType.Error:
                    Debug.LogError(taggedMsg);
                    break;
                case LogType.Exception:
                    Debug.LogException(new Exception(taggedMsg));
                    break;
                case LogType.Warning:
                    Debug.LogWarning(taggedMsg);
                    break;
                default:
                    Debug.Log(taggedMsg);
                    break;
            }
        }
    }
}
