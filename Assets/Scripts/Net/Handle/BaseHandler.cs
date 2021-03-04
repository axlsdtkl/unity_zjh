using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHandler
{
    //接收数据
    public abstract void OnReceive(int subCode, object value);

	
}
