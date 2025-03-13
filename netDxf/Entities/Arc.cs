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
using System.Collections.Generic;
using netDxf.Tables;

namespace netDxf.Entities
{
	/// <summary>Represents a circular arc <see cref="EntityObject">entity</see>.</summary>
	public class Arc :
		EntityObject
	{
		#region constructors

		/// <summary>Initializes a new instance of the class.</summary>
		public Arc()
			: this(Vector3.Zero, 1.0, 0.0, 180.0)
		{
		}
		/// <summary>Initializes a new instance of the class.</summary>
		/// <param name="center">Arc <see cref="Vector2">center</see> in world coordinates.</param>
		/// <param name="radius">Arc radius.</param>
		/// <param name="startAngle">Arc start angle in degrees.</param>
		/// <param name="endAngle">Arc end angle in degrees.</param>
		public Arc(Vector2 center, double radius, double startAngle, double endAngle)
			: this(new Vector3(center.X, center.Y, 0.0), radius, startAngle, endAngle)
		{
		}
		/// <summary>Initializes a new instance of the class.</summary>
		/// <param name="center">Arc <see cref="Vector3">center</see> in world coordinates.</param>
		/// <param name="radius">Arc radius.</param>
		/// <param name="startAngle">Arc start angle in degrees.</param>
		/// <param name="endAngle">Arc end angle in degrees.</param>
		public Arc(Vector3 center, double radius, double startAngle, double endAngle)
			: base(EntityType.Arc, DxfObjectCode.Arc)
		{
			this.Center = center;
			if (radius <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(radius), radius, "The arc radius must be greater than zero.");
			}
			_Radius = radius;
			_StartAngle = MathHelper.NormalizeAngle(startAngle);
			_EndAngle = MathHelper.NormalizeAngle(endAngle); 
		}
		/// <summary>Initializes a new instance of the class.</summary>
		/// <param name="startPoint">Arc start point.</param>
		/// <param name="endPoint">Arc end point.</param>
		/// <param name="bulge">Bulge value.</param>
		public Arc(Vector2 startPoint, Vector2 endPoint, double bulge)
			: base(EntityType.Arc, DxfObjectCode.Arc)
		{
			var (center, radius, startAngle, endAngle) = MathHelper.ArcFromBulge(startPoint, endPoint, bulge);
			if (radius <= 0)
			{
				throw new ApplicationException("The arc radius must be greater than zero.");
			}
			this.Center = new Vector3(center.X, center.Y, 0.0);
			_Radius = radius;
			_StartAngle = startAngle;
			_EndAngle = endAngle; 
		}

		#endregion

		#region public properties

		/// <summary>Gets or sets the arc <see cref="Vector3">center</see> in world coordinates.</summary>
		public Vector3 Center { get; set; }

		private double _Radius;
		/// <summary>Gets or sets the arc radius.</summary>
		public double Radius
		{
			get => _Radius;
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value), value, "The arc radius must be greater than zero.");
				}
				_Radius = value;
			}
		}

		private double _StartAngle;
		/// <summary>Gets or sets the arc start angle in degrees.</summary>
		public double StartAngle
		{
			get => _StartAngle;
			set => _StartAngle = MathHelper.NormalizeAngle(value);
		}

		private double _EndAngle;
		/// <summary>Gets or sets the arc end angle in degrees.</summary>
		public double EndAngle
		{
			get => _EndAngle;
			set => _EndAngle = MathHelper.NormalizeAngle(value);
		}

		/// <summary>Gets or sets the arc thickness.</summary>
		public double Thickness { get; set; } = 0.0;

		#endregion

		#region public methods

		/// <summary>Converts the arc in a list of vertexes.</summary>
		/// <param name="precision">Number of vertexes generated.</param>
		/// <returns>A list vertexes that represents the arc expressed in object coordinate system.</returns>
		public List<Vector2> PolygonalVertexes(int precision)
		{
			if (precision < 2)
			{
				throw new ArgumentOutOfRangeException(nameof(precision), precision, "The arc precision must be equal or greater than two.");
			}

			List<Vector2> ocsVertexes = new List<Vector2>();
			double start = _StartAngle * MathHelper.DegToRad;
			double end = _EndAngle * MathHelper.DegToRad;
			if (end < start)
			{
				end += MathHelper.TwoPI;
			}

			double delta = (end - start) / (precision - 1);
			for (int i = 0; i < precision; i++)
			{
				double angle = start + delta * i;
				double sine = _Radius * Math.Sin(angle);
				double cosine = _Radius * Math.Cos(angle);
				ocsVertexes.Add(new Vector2(cosine, sine));
			}

			return ocsVertexes;
		}

		/// <summary>Converts the arc in a Polyline2D.</summary>
		/// <param name="precision">Number of divisions.</param>
		/// <returns>A new instance of <see cref="Polyline2D"/> that represents the arc.</returns>
		public Polyline2D ToPolyline2D(int precision)
		{
			IEnumerable<Vector2> vertexes = this.PolygonalVertexes(precision);
			Vector3 ocsCenter = MathHelper.Transform(this.Center, this.Normal, CoordinateSystem.World, CoordinateSystem.Object);

			Polyline2D poly = new Polyline2D
			{
				Layer = (Layer)this.Layer.Clone(),
				Linetype = (Linetype)this.Linetype.Clone(),
				Color = (AciColor)this.Color.Clone(),
				Lineweight = this.Lineweight,
				Transparency = (Transparency)this.Transparency.Clone(),
				LinetypeScale = this.LinetypeScale,
				Normal = this.Normal,
				Elevation = ocsCenter.Z,
				Thickness = this.Thickness,
				IsClosed = false
			};
			foreach (Vector2 v in vertexes)
			{
				poly.Vertexes.Add(new Polyline2DVertex(v.X + ocsCenter.X, v.Y + ocsCenter.Y));
			}
			return poly;
		}

		#endregion

		#region overrides

		/// <inheritdoc/>
		public override void TransformBy(Matrix3 transformation, Vector3 translation)
		{
			Vector3 newCenter = transformation * this.Center + translation;
			Vector3 newNormal = transformation * this.Normal;
			if (Vector3.Equals(Vector3.Zero, newNormal))
			{
				newNormal = this.Normal;
			}

			Matrix3 transOW = MathHelper.ArbitraryAxis(this.Normal);
			Matrix3 transWO = MathHelper.ArbitraryAxis(newNormal).Transpose();

			Vector3 axis = transOW * new Vector3(this.Radius, 0.0, 0.0);
			axis = transformation * axis;
			axis = transWO * axis;
			Vector2 axisPoint = new Vector2(axis.X, axis.Y);
			double newRadius = axisPoint.Modulus();
			if (MathHelper.IsZero(newRadius))
			{
				newRadius = MathHelper.Epsilon;
			}

			Vector2 start = Vector2.Rotate(new Vector2(this.Radius, 0.0), this.StartAngle * MathHelper.DegToRad);
			Vector2 end = Vector2.Rotate(new Vector2(this.Radius, 0.0), this.EndAngle * MathHelper.DegToRad);

			Vector3 vStart = transOW * new Vector3(start.X, start.Y, 0.0);
			vStart = transformation * vStart;
			vStart = transWO * vStart;

			Vector3 vEnd = transOW * new Vector3(end.X, end.Y, 0.0);
			vEnd = transformation * vEnd;
			vEnd = transWO * vEnd;

			Vector2 startPoint = new Vector2(vStart.X, vStart.Y);
			Vector2 endPoint = new Vector2(vEnd.X, vEnd.Y);

			this.Normal = newNormal;
			this.Center = newCenter;
			this.Radius = newRadius;

			if (Math.Sign(transformation.M11 * transformation.M22 * transformation.M33) < 0)
			{
				this.EndAngle = Vector2.Angle(startPoint) * MathHelper.RadToDeg;
				this.StartAngle = Vector2.Angle(endPoint) * MathHelper.RadToDeg;
			}
			else
			{
				this.StartAngle = Vector2.Angle(startPoint) * MathHelper.RadToDeg;
				this.EndAngle = Vector2.Angle(endPoint) * MathHelper.RadToDeg;
			}
		}

		/// <inheritdoc/>
		public override object Clone()
		{
			Arc entity = new Arc
			{
				//EntityObject properties
				Layer = (Layer)this.Layer.Clone(),
				Linetype = (Linetype)this.Linetype.Clone(),
				Color = (AciColor)this.Color.Clone(),
				Lineweight = this.Lineweight,
				Transparency = (Transparency)this.Transparency.Clone(),
				LinetypeScale = this.LinetypeScale,
				Normal = this.Normal,
				IsVisible = this.IsVisible,
				//Arc properties
				Center = this.Center,
				Radius = _Radius,
				StartAngle = _StartAngle,
				EndAngle = _EndAngle,
				Thickness = this.Thickness
			};

			foreach (XData data in this.XData.Values)
			{
				entity.XData.Add((XData)data.Clone());
			}

			return entity;
		}

		#endregion
	}
}