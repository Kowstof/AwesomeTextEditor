// Author : Nikola Stepan
// E-mail : nikola.stepan@vz.htnet.hr
// Web    : http://calcsharp.net

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SimpleTextEditor
	{
	public class RicherTextBox1 : RichTextBox
		{
		private const int EM_SETCHARFORMAT = 1092;
		
		private const int CFM_BOLD = 1;
		private const int CFM_ITALIC = 2;
		private const int CFM_UNDERLINE = 4;
		
		private const int SCF_SELECTION = 1;

		[StructLayout( LayoutKind.Sequential )]
		private struct CHARFORMAT
			{
			public int cbSize;
			public uint dwMask;
			public uint dwEffects;
			public int yHeight;
			public int yOffset;
			public int crTextColor;
			public byte bCharSet;
			public byte bPitchAndFamily;
			[MarshalAs( UnmanagedType.ByValArray, SizeConst = 32 )]
			public char[] szFaceName;
		    
			// CHARFORMAT2 from here onwards.
			public short wWeight;
			public short sSpacing;
			public int crBackColor;
			public int LCID;
			public uint dwReserved;
			public short sStyle;
			public short wKerning;
			public byte bUnderlineType;
			public byte bAnimation;
			public byte bRevAuthor;
			}

		[DllImport( "user32", CharSet = CharSet.Auto )]
		private static extern int SendMessage( HandleRef hWnd, int msg, int wParam, ref CHARFORMAT lp );

		private void SetCharFormatMessage( ref CHARFORMAT fmt )
			{
			SendMessage(new HandleRef(this, Handle), EM_SETCHARFORMAT, SCF_SELECTION, ref fmt);
			}

		public void SetSelectionBoldOn()
			{
			ApplyStyle( CFM_BOLD, true );
			}

		public void SetSelectionBoldOff()
			{
			ApplyStyle( CFM_BOLD, false );
			}

		public void SetSelectionItalicOn()
			{
			ApplyStyle( CFM_ITALIC, true );
			}

		public void SetSelectionItalicOff()
			{
			ApplyStyle( CFM_ITALIC, false );
			}

		public void SetSelectionUnderlineOn()
			{
			ApplyStyle( CFM_UNDERLINE, true );
			}

		public void SetSelectionUnderlineOff()
			{
			ApplyStyle( CFM_UNDERLINE, false );
			}
			
		private void ApplyStyle( uint style, bool on )
			{
			CHARFORMAT fmt = new CHARFORMAT();
			fmt.cbSize = Marshal.SizeOf( fmt );
			
			fmt.dwMask = style;
			if ( on )
				fmt.dwEffects = style;

			SetCharFormatMessage( ref fmt );
			}
		}
	}