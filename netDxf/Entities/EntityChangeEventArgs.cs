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
	/// <summary>
	/// Represents the arguments thrown when the reference of an entity is added ore removed from another entity.
	/// </summary>
	public class EntityChangeEventArgs :
		EventArgs
	{
		#region private fields

		private readonly EntityObject item;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of <c>EntityChangeEventArgs</c>.
		/// </summary>
		/// <param name="item">The entity that is being added or removed from another entity.</param>
		public EntityChangeEventArgs(EntityObject item)
		{
			this.item = item;
		}

		#endregion

		#region public properties

		/// <summary>
		/// Gets the entity that is being added or removed.
		/// </summary>
		public EntityObject Item
		{
			get { return this.item; }
		}

		#endregion
	}
}