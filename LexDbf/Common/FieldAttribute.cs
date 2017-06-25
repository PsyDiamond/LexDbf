using System;

namespace LexTalionis.LexDbf.Common
{
    /// <summary>
    /// ������� ������������ ���� �������
    /// </summary>
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// ��� �������
        /// </summary>
        public char Type;
        /// <summary>
        /// ������
        /// </summary>
        public byte Length;
        /// <summary>
        /// ���������� ������ ����� �������
        /// </summary>
        public byte DecimalCount;
    }
}