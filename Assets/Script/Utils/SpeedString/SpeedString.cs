using UnityEngine;
using System.Collections;

public class SpeedString
{
    public string string_base;
    public System.Text.StringBuilder string_builder;
    private char[] int_parser = new char[11];

    public SpeedString(int capacity)
    {
        string_builder = new System.Text.StringBuilder(capacity, capacity);
        string_base = (string)string_builder.GetType().GetField(
            "_str",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance).GetValue(string_builder);
    }

    private int i;
    public void Clear()
    {
        string_builder.Length = 0;
        for (i = 0; i < string_builder.Capacity; i++)
        {
            string_builder.Append(' ');
        }
        string_builder.Length = 0;
    }

    public void OnlySetLen()
    {        
        string_builder.Length = 0;
    }
    //public void Draw(ref string text){
    //    text.text = "";
    //    text.text = string_base;
    //    text.cachedTextGenerator.Invalidate();
    //}

    public void Append(string value)
    {
        string_builder.Append(value);
    }
    public void Append(string value,int start,int len)
    {
        string_builder.Append(value, start, len);
    }

    public int count;
 
    public void AppendLast(int value)
    {
        if (value >= 0)
        {
            count = ToCharArrayLast((uint)value, int_parser, 0);
        }
        else
        {
            count = ToCharArrayLast((uint)-value, int_parser, 1) + 1;
        }
        for (i = 0; i < count; i++)
        {
            string_builder.Append(int_parser[i]);
        }
    }
    public void Append(char c)
    {
        string_builder.Append(c);
    }
    public void AppendHUD(int value)
    {
        bool nagtive = false;
        if (value < 0)
        {
            value = -value;
            nagtive = true;
        }
        count = ToCharArray((uint)value, int_parser, 0);
        int start= (int)((string_builder.Capacity - count) * 0.5);

        for (i = 0; i < start; i++)
        {
            string_builder.Append(' ');
        }
        if(nagtive)
            string_builder.Append('s');
        for (i = 0; i < count; i++)
        {
            string_builder.Append(int_parser[i]);
        }
    }
    public void Append(int value)
    {
        if (value >= 0)
        {
            count = ToCharArray((uint)value, int_parser, 0);
        }
        else
        {
            count = ToCharArray((uint)-value, int_parser, 1) + 1;
        }
        for (i = 0; i < count; i++)
        {
            string_builder.Append(int_parser[i]);
        }
    }
    public void Change(System.Text.StringBuilder sb,int _count)
    {
        
        for (i = 0; i < _count; i++)
        {
            string_builder.Append(sb[i]);
        }
        count = _count;        
    }
    private static int ToCharArray(uint value, char[] buffer, int bufferIndex)
    {
        if (value == 0)
        {
            buffer[bufferIndex] = '0';
            return 1;
        }
        int len = 1;
        for (uint rem = value / 10; rem > 0; rem /= 10)
        {
            len++;
        }
        for (int i = len - 1; i >= 0; i--)
        {
            buffer[bufferIndex + i] = (char)('0' + (value % 10));
            value /= 10;
        }
        return len;
    }

    private static int ToCharArrayLast(uint value, char[] buffer, int bufferIndex,int _defult=6)
    {        
        int len = 1;
        for (uint rem = value / 10; rem > 0; rem /= 10)
        {
            len++;
        }
        
        for (int i = 0; i < _defult; i++)
        {
            if (i<len)
            {
                buffer[(_defult - 1) - i] = (char)('0' + (value % 10));
                value /= 10;
            }
            else
            {
                buffer[(_defult - 1) - i] = (char)('0');
            }
            
        }
        return _defult;
    }

    public void ToLower()
    {
        var len= string_builder.Length;
        for (int i = 0; i < len; i++)
        {
            var key = string_builder[i];
            if ((key >= 'A') && (key <= 'Z'))
            {
                string_builder[i] = (char)(key + ('a' - 'A'));
            }
        }
    }
}
