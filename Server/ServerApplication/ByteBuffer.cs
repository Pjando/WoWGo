using System;
using System.Collections.Generic;
using System.Text;

/*! 
 *  \author    Kevin Kaymak
 */

/// <summary>
/// Used to serialise data into a byte array to send over the network.
/// </summary>
public class ByteBuffer : IDisposable {

    /// <summary>
    /// List of type byte used to write data.
    /// </summary>
    List<byte> buff;

    /// <summary>
    /// Byte array used to read values from the buffer.
    /// </summary>
    byte[] readBuff;

    /// <summary>
    /// The index in the array that is being read from.
    /// </summary>
    int readpos;

    /// <summary>
    /// Indicates whether the buffer has been written to.
    /// </summary>
    bool buffUpdate = false;

    /// <summary>
    /// Creates a Byte Buffer object initialsing readpos to 0.
    /// </summary>
    public ByteBuffer() {
        buff = new List<byte>();
        readpos = 0;
    }

    /// <summary>
    /// Get the current read position of the buffer.
    /// </summary>
    /// <returns>Integer of the read position.</returns>
    public int GetReadPos() {
        return readpos;
    }

    /// <summary>
    /// Get a byte array of the data in the buffer.
    /// </summary>
    /// <returns>Byte array containg buffer data.</returns>
    public byte[] ToArray() {
        return buff.ToArray();
    }

    /// <summary>
    /// Gets the number of elements in the buff list.
    /// </summary>
    /// <returns>Integer number of elements.</returns>
    public int Count() {
        return buff.Count;
    }

    /// <summary>
    /// The length of buffer from the read position.
    /// </summary>
    /// <returns>Integer length Count()-readpos</returns>
    public int Length() {
        return Count() - readpos;
    }

    /// <summary>
    /// Clears the buffer and resets read position to 0.
    /// </summary>
    public void Clear() {
        buff.Clear();
        readpos = 0;
    }

    #region"Write Data"

    /// <summary>
    /// Writes a byte to the buffer.
    /// </summary>
    /// <param name="input">Byte to be written to the buffer.</param>
    public void WriteByte(byte input) {
        buff.Add(input);
        buffUpdate = true;
    }

    /// <summary>
    /// Writes a byte array to the buffer.
    /// </summary>
    /// <param name="input">Byte array to be written to buffer.</param>
    public void WriteBytes(byte[] input) {
        buff.AddRange(input);
        buffUpdate = true;
    }

    /// <summary>
    /// Converts a short to a byte array and writes it to the buffer.
    /// </summary>
    /// <param name="input">Short variable to be written to the buffer.</param>
    public void WriteShort(short input) {
        buff.AddRange(BitConverter.GetBytes(input));
        buffUpdate = true;
    }

    /// <summary>
    /// Converts an integer to a byte array and writes it to the buffer.
    /// </summary>
    /// <param name="input">Integer to be written to the buffer.</param>
    public void WriteInteger(int input) {
        buff.AddRange(BitConverter.GetBytes(input));
        buffUpdate = true;
    }

    /// <summary>
    /// Converts a float to a byte array and writes it to the buffer.
    /// </summary>
    /// <param name="input">Float to be written to the buffer.</param>
    public void WriteFloat(float input) {
        buff.AddRange(BitConverter.GetBytes(input));
        buffUpdate = true;
    }

    /// <summary>
    /// Converts a string to a byte array and writes it to the buffer.
    /// </summary>
    /// <param name="input">String to be written to the buffer.</param>
    public void WriteString(string input) {
        buff.AddRange(BitConverter.GetBytes(input.Length));
        buff.AddRange(Encoding.ASCII.GetBytes(input));
        buffUpdate = true;
    }
    #endregion

    #region "Read Data"
    /// <summary>
    /// Reads an integer from the buffer.
    /// </summary>
    /// <param name="peek">Whether the read position should be incremented after reading. True by default.</param>
    /// <returns>Returns integer from the buffer.</returns>
    public int ReadInteger(bool peek = true) {
        if (buff.Count > readpos) {
            if (buffUpdate) {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            int ret = BitConverter.ToInt32(readBuff, readpos);
            if (peek && buff.Count > readpos) {
                readpos += 4;
            }

            return ret;
        } else {
            throw new Exception("Byte Buffer is past its limit!");
        }
    }

    /// <summary>
    /// Reads a string from the buffer.
    /// </summary>
    /// <param name="peek">Whether the read position should be incremented after reading. True by default.</param>
    /// <returns>Returns a string from the buffer.</returns>
    public string ReadString(bool peek = true) {
        int len = ReadInteger(true);
        if (buffUpdate) {
            readBuff = buff.ToArray();
            buffUpdate = false;
        }

        string ret = Encoding.ASCII.GetString(readBuff, readpos, len);

        if (peek && buff.Count > readpos) {
            if (ret.Length > 0) {
                readpos += len;
            }
        }

        return ret;
    }

    /// <summary>
    /// Reads a byte from the buffer.
    /// </summary>
    /// <param name="peek">Whether the read position should be incremented after reading. True by default.</param>
    /// <returns>Returns a byte from the buffer.</returns>
    public byte ReadByte(bool peek = true) {
        if (buff.Count > readpos) {
            if (buffUpdate) {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            byte ret = readBuff[readpos];
            if (peek && buff.Count > readpos) {
                readpos += 1;
            }

            return ret;

        } else {
            throw new Exception("Byte Buffer is past its limit!");
        }
    }

    /// <summary>
    /// Reads a byte array from the buffer.
    /// </summary>
    /// <param name="length">The length of the byte array to be read.</param>
    /// <param name="peek">Whether the read position should be incremented after reading. True by default.</param>
    /// <returns>Returns a byte array from the buffer.</returns>
    public byte[] ReadBytes(int length, bool peek = true) {
        if (buffUpdate) {
            readBuff = buff.ToArray();
            buffUpdate = false;
        }

        byte[] ret = buff.GetRange(readpos, length).ToArray();

        if (peek) {
            readpos += length;
        }

        return ret;
    }

    /// <summary>
    /// Reads a float from the buffer.
    /// </summary>
    /// <param name="peek">Whether the read position should be incremented after reading. True by default.</param>
    /// <returns>Returns a float from the buffer.</returns>
    public float ReadFloat(bool peek = true) {
        if (buff.Count > readpos) {
            if (buffUpdate) {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            float ret = BitConverter.ToSingle(readBuff, readpos);
            if (peek && buff.Count > readpos) {
                readpos += 4;
            }

            return ret;
        } else {
            throw new Exception("Byte Buffer is past its limit!");
        }
    }
    #endregion

    /// <summary>
    /// Track whether Dispose has been called.
    /// </summary>
    private bool disposedValue = false;

    /// <summary>
    /// Disposes unmanaged and managed resources.
    /// </summary>
    /// <param name="disposing">Disposes both managed and unmanaged resources if true, only unmanaged if false.</param>
    protected virtual void Dispose(bool disposing) {
        if (!this.disposedValue) {
            if (disposing) {
                buff.Clear();
            }

            readpos = 0;
        }
        this.disposedValue = true;
    }

    /// <summary>
    /// Public method for virtual dispose() method.
    /// </summary>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
