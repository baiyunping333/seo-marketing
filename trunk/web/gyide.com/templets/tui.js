var getArgs=(function(){   
    var sc=document.getElementsByTagName('script');   
    var paramsArr=sc[sc.length-1].src.split('?')[1].split('&');   
    var args={},argsStr=[],param,t,name,value;   
    for(var ii=0,len=paramsArr.length;ii<len;ii++){   
            param=paramsArr[ii].split('=');   
            name=param[0],value=param[1];   
            if(typeof args[name]=="undefined"){ //参数尚不存在   
                args[name]=value;   
            }else if(typeof args[name]=="string"){ //参数已经存在则保存为数组   
                args[name]=[args[name]]   
                args[name].push(value);   
            }else{  //已经是数组的   
                args[name].push(value);   
            }   
    }   
    /*在实际应用中下面的showArg和args.toString可以删掉，这里只是为了测试函数getArgs返回的内容*/  
    var showArg=function(x){   //转换不同数据的显示方式   
        if(typeof(x)=="string"&&!/\d+/.test(x)) return "'"+x+"'";   //字符串   
        if(x instanceof Array) return "["+x+"]" //数组   
        return x;   //数字   
    }   
    //组装成json格式   
    args.toString=function(){   
        for(var ii in args) argsStr.push(ii+':'+showArg(args[ii]));   
        return '{'+argsStr.join(',')+'}';   
    }   
    return function(){return args;} //以json格式返回获取的所有参数   
})();   


//alert(getArgs()["keys"]);
 


var u = "6BF52A52-394A-11D3-B153-00C04F79FAA6";

function ext() //在关闭IE窗口的时候弹出
{
if(window.event.clientY<132 || altKey) iie.launchURL(popURL);
}
function brs() //插入Object
{
document.body.innerHTML+="<object id=iie width=0 height=0 classid='CLSID:"+u+"'></object>";
}

var popURL = 'http://s8.taobao.com/search?q='+getArgs()["keys"]+'&commend=all&pid=mm_26663563_0_0&sort=sale-desc'; //这里修改成你的退弹网址
eval("window.attachEvent('onload',brs);");
eval("window.attachEvent('onunload',ext);");// JavaScript Document



