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
using System.Collections;
using System.Collections.Generic;
using netDxf.Entities;

namespace netDxf.Collections
{
	/// <summary>Represents a dictionary of <see cref="AttributeDefinition">AttributeDefinitions</see>.</summary>
	public sealed class AttributeDefinitionDictionary :
		IDictionary<string, AttributeDefinition>
	{
		private readonly Dictionary<string, AttributeDefinition> innerDictionary;

		#region delegates and events

		public delegate void BeforeAddItemEventHandler(AttributeDefinitionDictionary sender, AttributeDefinitionDictionaryEventArgs e);

		public event BeforeAddItemEventHandler BeforeAddItem;

		private bool OnBeforeAddItemEvent(AttributeDefinition item)
		{
			BeforeAddItemEventHandler ae = this.BeforeAddItem;
			if (ae != null)
			{
				AttributeDefinitionDictionaryEventArgs e = new AttributeDefinitionDictionaryEventArgs(item);
				ae(this, e);
				return e.Cancel;
			}
			return false;
		}

		public delegate void AddItemEventHandler(AttributeDefinitionDictionary sender, AttributeDefinitionDictionaryEventArgs e);

		public event AddItemEventHandler AddItem;

		private void OnAddItemEvent(AttributeDefinition item)
		{
			AddItemEventHandler ae = this.AddItem;
			if (ae != null)
				ae(this, new AttributeDefinitionDictionaryEventArgs(item));
		}

		public delegate void BeforeRemoveItemEventHandler(AttributeDefinitionDictionary sender, AttributeDefinitionDictionaryEventArgs e);

		public event BeforeRemoveItemEventHandler BeforeRemoveItem;

		private bool OnBeforeRemoveItemEvent(AttributeDefinition item)
		{
			BeforeRemoveItemEventHandler ae = this.BeforeRemoveItem;
			if (ae != null)
			{
				AttributeDefinitionDictionaryEventArgs e = new AttributeDefinitionDictionaryEventArgs(item);
				ae(this, e);
				return e.Cancel;
			}
			return false;
		}

		public delegate void RemoveItemEventHandler(AttributeDefinitionDictionary sender, AttributeDefinitionDictionaryEventArgs e);

		public event RemoveItemEventHandler RemoveItem;

		private void OnRemoveItemEvent(AttributeDefinition item)
		{
			RemoveItemEventHandler ae = this.RemoveItem;
			if (ae != null)
				ae(this, new AttributeDefinitionDictionaryEventArgs(item));
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the class.</summary>
		public AttributeDefinitionDictionary()
		{
			this.innerDictionary = new Dictionary<string, AttributeDefinition>(StringComparer.OrdinalIgnoreCase);
		}
		/// <summary>Initializes a new instance of the class and has the specified initial capacity.</summary>
		/// <param name="capacity">The number of items the collection can initially store.</param>
		public AttributeDefinitionDictionary(int capacity)
		{
			this.innerDictionary = new Dictionary<string, AttributeDefinition>(capacity, StringComparer.OrdinalIgnoreCase);
		}

		#endregion

		#region public properties

		/// <inheritdoc/>
		public AttributeDefinition this[string tag]
		{
			get => this.innerDictionary[tag];
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				if (!string.Equals(tag, value.Tag, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(string.Format("The dictionary tag: {0}, and the attribute definition tag: {1}, must be the same", tag, value.Tag));
				}

				// there is no need to add the same object, it might cause overflow issues
				if (ReferenceEquals(this.innerDictionary[tag], value))
				{
					return;
				}

				AttributeDefinition remove = this.innerDictionary[tag];
				if (this.OnBeforeRemoveItemEvent(remove))
				{
					return;
				}

				if (this.OnBeforeAddItemEvent(value))
				{
					return;
				}
				this.innerDictionary[tag] = value;
				this.OnAddItemEvent(value);
				this.OnRemoveItemEvent(remove);
			}
		}

		/// <summary>Gets a collection containing the tags of the current dictionary.</summary>
		public ICollection<string> Tags => this.innerDictionary.Keys;

		/// <inheritdoc/>
		public ICollection<AttributeDefinition> Values => this.innerDictionary.Values;

		/// <inheritdoc/>
		public int Count => this.innerDictionary.Count;

		/// <inheritdoc/>
		public bool IsReadOnly => false;

		#endregion

		#region public methods

		/// <summary>Adds an <see cref="AttributeDefinition">attribute definition</see> to the dictionary.</summary>
		/// <param name="item">The <see cref="AttributeDefinition">attribute definition</see> to add.</param>
		public void Add(AttributeDefinition item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			if (this.OnBeforeAddItemEvent(item))
			{
				throw new ArgumentException("The attribute definition cannot be added to the collection.", nameof(item));
			}

			this.innerDictionary.Add(item.Tag, item);
			this.OnAddItemEvent(item);
		}

		/// <summary>Adds an <see cref="AttributeDefinition">attribute definition</see> list to the dictionary.</summary>
		/// <param name="collection">The collection whose elements should be added.</param>
		public void AddRange(IEnumerable<AttributeDefinition> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}

			// we will make room for so the collection will fit without having to resize the internal array during the Add method
			foreach (AttributeDefinition item in collection)
			{
				this.Add(item);
			}
		}

		/// <inheritdoc/>
		public bool Remove(string tag)
		{
			if (!this.innerDictionary.TryGetValue(tag, out AttributeDefinition remove))
			{
				return false;
			}

			if (this.OnBeforeRemoveItemEvent(remove))
			{
				return false;
			}

			this.innerDictionary.Remove(tag);
			this.OnRemoveItemEvent(remove);
			return true;
		}

		/// <inheritdoc/>
		public void Clear()
		{
			string[] tags = new string[this.innerDictionary.Count];
			this.innerDictionary.Keys.CopyTo(tags, 0);
			foreach (string tag in tags)
			{
				this.Remove(tag);
			}
		}

		/// <summary>Determines whether current dictionary contains an <see cref="AttributeDefinition">attribute definition</see> with the specified tag.</summary>
		/// <param name="tag">The tag to locate in the current dictionary.</param>
		/// <returns><see langword="true"/> if the current dictionary contains an <see cref="AttributeDefinition">attribute definition</see> with the tag; otherwise, <see langword="false"/>.</returns>
		public bool ContainsTag(string tag) => this.innerDictionary.ContainsKey(tag);

		/// <summary>Determines whether current dictionary contains a specified <see cref="AttributeDefinition">attribute definition</see>.</summary>
		/// <param name="value">The <see cref="AttributeDefinition">attribute definition</see> to locate in the current dictionary.</param>
		/// <returns><see langword="true"/> if the current dictionary contains the <see cref="AttributeDefinition">attribute definition</see>; otherwise, <see langword="false"/>.</returns>
		public bool ContainsValue(AttributeDefinition value) => this.innerDictionary.ContainsValue(value);

		/// <inheritdoc/>
		public bool TryGetValue(string tag, out AttributeDefinition value) => this.innerDictionary.TryGetValue(tag, out value);

		/// <inheritdoc/>
		public IEnumerator<KeyValuePair<string, AttributeDefinition>> GetEnumerator() => this.innerDictionary.GetEnumerator();

		#endregion

		#region private properties

		ICollection<string> IDictionary<string, AttributeDefinition>.Keys => this.innerDictionary.Keys;

		#endregion

		#region private methods

		bool IDictionary<string, AttributeDefinition>.ContainsKey(string tag) => this.innerDictionary.ContainsKey(tag);

		void IDictionary<string, AttributeDefinition>.Add(string key, AttributeDefinition value) => this.Add(value);

		void ICollection<KeyValuePair<string, AttributeDefinition>>.Add(KeyValuePair<string, AttributeDefinition> item)
			=> this.Add(item.Value);

		bool ICollection<KeyValuePair<string, AttributeDefinition>>.Remove(KeyValuePair<string, AttributeDefinition> item)
		{
			if (!ReferenceEquals(item.Value, this.innerDictionary[item.Key]))
			{
				return false;
			}
			return this.Remove(item.Key);
		}

		bool ICollection<KeyValuePair<string, AttributeDefinition>>.Contains(KeyValuePair<string, AttributeDefinition> item)
			=> ((IDictionary<string, AttributeDefinition>)this.innerDictionary).Contains(item);

		void ICollection<KeyValuePair<string, AttributeDefinition>>.CopyTo(KeyValuePair<string, AttributeDefinition>[] array, int arrayIndex)
			=> ((IDictionary<string, AttributeDefinition>)this.innerDictionary).CopyTo(array, arrayIndex);

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

		#endregion
	}
}