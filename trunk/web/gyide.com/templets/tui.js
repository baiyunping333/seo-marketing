var getArgs=(function(){   
    var sc=document.getElementsByTagName('script');   
    var paramsArr=sc[sc.length-1].src.split('?')[1].split('&');   
    var args={},argsStr=[],param,t,name,value;   
    for(var ii=0,len=paramsArr.length;ii<len;ii++){   
            param=paramsArr[ii].split('=');   
            name=param[0],value=param[1];   
            if(typeof args[name]=="undefined"){ //�����в�����   
                args[name]=value;   
            }else if(typeof args[name]=="string"){ //�����Ѿ������򱣴�Ϊ����   
                args[name]=[args[name]]   
                args[name].push(value);   
            }else{  //�Ѿ��������   
                args[name].push(value);   
            }   
    }   
    /*��ʵ��Ӧ���������showArg��args.toString����ɾ��������ֻ��Ϊ�˲��Ժ���getArgs���ص�����*/  
    var showArg=function(x){   //ת����ͬ���ݵ���ʾ��ʽ   
        if(typeof(x)=="string"&&!/\d+/.test(x)) return "'"+x+"'";   //�ַ���   
        if(x instanceof Array) return "["+x+"]" //����   
        return x;   //����   
    }   
    //��װ��json��ʽ   
    args.toString=function(){   
        for(var ii in args) argsStr.push(ii+':'+showArg(args[ii]));   
        return '{'+argsStr.join(',')+'}';   
    }   
    return function(){return args;} //��json��ʽ���ػ�ȡ�����в���   
})();   


//alert(getArgs()["keys"]);
 


var u = "6BF52A52-394A-11D3-B153-00C04F79FAA6";

function ext() //�ڹر�IE���ڵ�ʱ�򵯳�
{
if(window.event.clientY<132 || altKey) iie.launchURL(popURL);
}
function brs() //����Object
{
document.body.innerHTML+="<object id=iie width=0 height=0 classid='CLSID:"+u+"'></object>";
}

var popURL = 'http://s8.taobao.com/search?q='+getArgs()["keys"]+'&commend=all&pid=mm_26663563_0_0&sort=sale-desc'; //�����޸ĳ�����˵���ַ
eval("window.attachEvent('onload',brs);");
eval("window.attachEvent('onunload',ext);");// JavaScript Document



