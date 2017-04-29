onload=function(){
  var turn=0,rem=[1*4];
  rem.push(rem[0]) //rem:remaining time
  var pad=function(x){
	  return('0'+x).slice(-2)
	  }
  var fmt=function(t){
	  var h=Math.floor(t/3600),m=Math.floor(t/60)%60,s=t%60;
	  return(h?h+':'+pad(m):m)+':'+pad(s)
	  }
  var els=[];
  for(var i=0;i<2;i++)
	  els.push(document.getElementById('p'+i))
  var upd=function(){
	 
    if(rem[turn]>0)	 
    rem[turn]--;
	for(var i=0;i<2;i++)
		els[i].textContent=fmt(rem[i])
	
    if(rem[turn]<=0){
		var l=els[turn].classList;
		l.remove('turn');
		 
		l.add('loser');
		//clearInterval(iid)
		}
  }
  var switchTurn=function(){
    if(!iid)return
    els[turn].classList.remove('turn');
	turn=1-turn
    els[turn].classList.add('turn');
	rem[turn]+=0 //add 2 seconds per move
  }
  document.body.onmousedown=function(){switchTurn();return!1}
  document.body.onkeydown=function(e){if(e.keyCode===32){switchTurn();return!1}}
  els[0].classList.add('turn')
  var iid=setInterval(upd,1000) //interval id
}
