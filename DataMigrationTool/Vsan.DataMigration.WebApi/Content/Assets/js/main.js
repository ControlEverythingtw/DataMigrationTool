layui.config({
	base : "/Assets/js/"
}).use(['form','element','layer','jquery'],function(){
	var form = layui.form,
		layer = parent.layer === undefined ? layui.layer : parent.layer,
		element = layui.element,
		$ = layui.jquery;
	$(".panel a").on("click",function(){
		window.parent.addTab($(this));
	})
	////动态获取文章总数和待审核文章数量,最新文章
	//$.get("/Assets/json/newsList.json",
	//	function(data){
	//		var waitNews = [];
	//		$(".allNews span").text(data.length);  //文章总数
	//		for(var i=0;i<data.length;i++){
	//			var newsStr = data[i];
	//			if(newsStr["newsStatus"] == "待审核"){
	//				waitNews.push(newsStr);
	//			}
	//		}
	//		$(".waitNews span").text(waitNews.length);  //待审核文章
	//		//加载最新文章
	//		var hotNewsHtml = '';
	//		for(var i=0;i<5;i++){
	//			hotNewsHtml += '<tr>'
	//	    	+'<td align="left">'+data[i].newsName+'</td>'
	//	    	+'<td>'+data[i].newsTime+'</td>'
	//	    	+'</tr>';
	//		}
	//		$(".hot_news").html(hotNewsHtml);
	//	}
	//)
	//数字格式化
	$(".panel span").each(function(){
		$(this).html($(this).text()>9999 ? ($(this).text()/10000).toFixed(2) + "<em>万</em>" : $(this).text());	
	})
	//系统基本参数
	if(window.sessionStorage.getItem("systemParameter")){
		var systemParameter = JSON.parse(window.sessionStorage.getItem("systemParameter"));
		fillParameter(systemParameter);
	}else{
		$.ajax({
			url : "/Assets/json/systemParameter.json",
			type : "get",
			dataType : "json",
			success : function(data){
				fillParameter(data);
			}
		})
	}
	//填充数据方法
 	function fillParameter(data){
 		//判断字段数据是否存在
 		function nullData(data){
 			if(data == '' || data == "undefined"){
 				return "未定义";
 			}else{
 				return data;
 			}
 		}
 	}

})
