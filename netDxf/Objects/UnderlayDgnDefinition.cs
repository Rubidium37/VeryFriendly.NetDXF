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

using System.Collections.Generic;
using System.IO;
using netDxf.Collections;
using netDxf.Tables;

namespace netDxf.Objects
{
	/// <summary>Represents a <b>DGN</b> underlay definition.</summary>
	public class UnderlayDgnDefinition :
		UnderlayDefinition
	{
		#region constructor

		/// <summary>Initializes a new instance of the class.</summary>
		/// <param name="file">Underlay file name with full or relative path.</param>
		/// <remarks>
		/// The file extension must match the underlay type.
		/// </remarks>
		public UnderlayDgnDefinition(string file)
			: this(Path.GetFileNameWithoutExtension(file), file)
		{
		}
		/// <summary>Initializes a new instance of the class.</summary>
		/// <param name="name">Underlay definition name.</param>
		/// <param name="file">Underlay file name with full or relative path.</param>
		/// <remarks>
		/// The file extension must match the underlay type.
		/// </remarks>
		public UnderlayDgnDefinition(string name, string file)
			: base(name, file, UnderlayType.DGN)
		{
			this.Layout = "Model";
		}

		#endregion

		#region public properties

		/// <summary>Gets or sets the layout name to show.</summary>
		public string Layout { get; set; }

		/// <summary>Gets the owner of the actual underlay <b>DGN</b> definition.</summary>
		public new UnderlayDgnDefinitions Owner
		{
			get => (UnderlayDgnDefinitions)base.Owner;
			internal set => base.Owner = value;
		}

		#endregion

		#region overrides

		/// <inheritdoc/>
		public override bool HasReferences() => this.Owner != null && this.Owner.HasReferences(this.Name);

		/// <inheritdoc/>
		public override List<DxfObjectReference> GetReferences()
		{
			if (this.Owner == null)
			{
				return null;
			}

			return this.Owner.GetReferences(this.Name);
		}

		/// <inheritdoc/>
		public override TableObject Clone(string newName)
		{
			UnderlayDgnDefinition copy = new UnderlayDgnDefinition(newName, this.File)
			{
				Layout = this.Layout
			};

			foreach (XData data in this.XData.Values)
			{
				copy.XData.Add((XData)data.Clone());
			}

			return copy;
		}
		/// <inheritdoc/>
		public override object Clone() => this.Clone(this.Name);

		#endregion
	}
}