using Adaptive.Intelligence.Security.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security.Memory
{
    public class SecureMemoryItemBaseTMock : SecureMemoryItemBase<int>
    {
        protected override int TranslateValueFromBytes(byte[]? content)
        {
            if (content == null || content.Length != 4)
            {
                throw new ArgumentException("Invalid byte array for integer conversion.");
            }
            return BitConverter.ToInt32(content, 0);
        }

        protected override byte[]? TranslateValueToBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public void Clear()
        {
            base.ClearStorage();
        }
    }
}
