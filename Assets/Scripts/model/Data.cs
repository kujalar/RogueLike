using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData<T>  {

	T data;
	public bool isDirty = true;

	public void setData(T value){
		data = value;
		isDirty = true;
	}
	public T getData(){
		return data;
	}
}
