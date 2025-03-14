#region netDxf library licensed under the MIT License
//
//                       netDxf library
// Copyright (c) Daniel Carvajal (haplokuon@gmail.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
#endregion

using System;

namespace netDxf.Entities
{
	/// <summary>viewport status flags</summary>
	[Flags]
	public enum ViewportStatusFlags
	{
		/// <summary>Enables perspective mode.</summary>
		PerspectiveMode = 1,

		/// <summary>Enables front clipping.</summary>
		FrontClipping = 2,

		/// <summary>Enables back clipping.</summary>
		BackClipping = 4,

		/// <summary>Enables <b>UCS</b> follow.</summary>
		UcsFollow = 8,

		/// <summary>Enables front clip not at eye.</summary>
		FrontClipNotAtEye = 16,

		/// <summary>Enables <b>UCS</b> icon visibility.</summary>
		UcsIconVisibility = 32,

		/// <summary>Enables <b>UCS</b> icon at origin.</summary>
		UcsIconAtOrigin = 64,

		/// <summary>Enables fast zoom.</summary>
		FastZoom = 128,

		/// <summary>Enables snap mode.</summary>
		SnapMode = 256,

		/// <summary>Enables grid mode.</summary>
		GridMode = 512,

		/// <summary>Enables isometric snap style.</summary>
		IsometricSnapStyle = 1024,

		/// <summary>Enables hide plot mode.</summary>
		HidePlotMode = 2048,

		/// <summary>If set and IsoPairRight is not set, then isopair top is enabled. If both IsoPairTop and IsoPairRight are set, then isopair left is enabled</summary>
		IsoPairTop = 4096,

		/// <summary>If set and IsoPairTop is not set, then isopair right is enabled.</summary>
		IsoPairRight = 8192,

		/// <summary>Enables viewport zoom locking.</summary>
		ViewportZoomLocking = 16384,

		/// <summary>Currently always enabled.</summary>
		CurrentlyAlwaysEnabled = 32768,

		/// <summary>Enables non-rectangular clipping.</summary>
		NonRectangularClipping = 65536,

		/// <summary>Turns the viewport off.</summary>
		ViewportOff = 131072,

		/// <summary>Enables the display of the grid beyond the drawing limits.</summary>
		DisplayGridBeyondDrawingLimits = 262144,

		/// <summary>Enable adaptive grid display.</summary>
		AdaptiveGridDisplay = 524288,

		/// <summary>Enables subdivision of the grid below the set grid spacing when the grid display is adaptive.</summary>
		SubdivisionGridBelowSpacing = 1048576
	}
}