var Utils={
	reverse:function(inStr){
		return inStr.split("").reverse().join("")
	},
	/*
	Decode:
	Utils.AFdecode('alert("1234")')
	*/
	AFdecode:function(inPackerScript){
		return escape(Utils.reverse(inPackerScript));
	},
	/*
	Exec:
	Utils.AFexec("%29%224321%22%28trela")
	*/
	AFexec:function(inAFdecodeScript){
		eval(Utils.reverse(unescape(inAFdecodeScript)));
	}
};