using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData<T>  {

    //use for quick reading, without checking dirtyness
	public T data;
	public bool isDirty = true;

    //set with this, cause this will set Instance dirty
	public void setData(T value){
		data = value;
		isDirty = true;
	}
    //TODO make a method that tells if the data was dirty
	public bool wasDirtyAndGetClean(out T value){
        value = data;
        bool output = isDirty;
        isDirty = false;
		return output;
	}
}
