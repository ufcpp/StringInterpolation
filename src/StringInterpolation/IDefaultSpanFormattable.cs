using System.Buffers;

namespace StringInterpolation;

public interface IDefaultSpanFormattable : ISpanFormattable
{
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        var buffer = (stackalloc char[512]);
        if (TryFormat(buffer, out var charsWritten, format, formatProvider))
        {
            return new string(buffer[..charsWritten]);
        }

        int size = 1024;
        while (true)
        {
            var array = ArrayPool<char>.Shared.Rent(size);
            string? s = null;
            if (TryFormat(array, out charsWritten, format, formatProvider))
            {
                s = new string(array[..charsWritten]);
            }
            ArrayPool<char>.Shared.Return(array);

            if (s != null) return s;

            size *= 2;
        }
    }
}
