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
	/// <summary>Represents datum reference, a theoretically exact point, axis, or plane from which you make measurements and verify dimensions.</summary>
	public class DatumReferenceValue :
		ICloneable
	{
		#region constructors

		/// <summary>Initializes a new instance of the class.</summary>
		public DatumReferenceValue()
		{
			this.Value = string.Empty;
			this.MaterialCondition = ToleranceMaterialCondition.None;
		}
		/// <summary>Initializes a new instance of the class.</summary>
		/// <param name="value">Datum reference value.</param>
		/// <param name="materialCondition">Datum material condition.</param>
		public DatumReferenceValue(string value, ToleranceMaterialCondition materialCondition)
		{
			this.Value = value;
			this.MaterialCondition = materialCondition;
		}

		#endregion

		#region public properties

		/// <summary>Gets or sets the datum value.</summary>
		public string Value { get; set; }

		/// <summary>Gets or sets the datum material condition.</summary>
		public ToleranceMaterialCondition MaterialCondition { get; set; }

		#endregion

		#region ICloneable

		/// <inheritdoc/>
		public object Clone()
			=> new DatumReferenceValue
			{
				Value = this.Value,
				MaterialCondition = this.MaterialCondition
			};

		#endregion
	}
}