using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMathf
{
	public static Vector3 RemoveDotVector(Vector3 _vector, Vector3 _direction)
	{
		if (_direction.sqrMagnitude != 1)
			_direction.Normalize();

		float _amount = Vector3.Dot(_vector, _direction);

		_vector -= _direction * _amount;

		return _vector;
	}
	public static Vector3 ExtractDotVector(Vector3 _vector, Vector3 _direction)
	{
		if (_direction.sqrMagnitude != 1)
			_direction.Normalize();

		float _amount = Vector3.Dot(_vector, _direction);

		return _direction * _amount;
	}
}
