(function(n,t){typeof define=="function"&&define.amd?define([],t):typeof module=="object"&&module.exports?module.exports=t():n.PDFObject=t()})(this,function(){"use strict";if(typeof window=="undefined"||typeof navigator=="undefined")return!1;var v="2.1.1",t=window.navigator.userAgent,i,e,y=typeof navigator.mimeTypes["application/pdf"]!="undefined",o,p=function(){return typeof Promise!="undefined"}(),w=function(){return t.indexOf("irefox")!==-1}(),b=function(){return w?parseInt(t.split("rv:")[1].split(".")[0],10)>18:!1}(),r=function(){return/iphone|ipad|ipod/i.test(t.toLowerCase())}(),u,s,h,n,c,l,f,a;return u=function(n){var t;try{t=new ActiveXObject(n)}catch(i){t=null}return t},e=function(){return!!(window.ActiveXObject||"ActiveXObject"in window)},o=function(){return!!(u("AcroPDF.PDF")||u("PDF.PdfCtrl"))},i=!r&&(b||y||e()&&o()),s=function(n){var t="",i;if(n){for(i in n)n.hasOwnProperty(i)&&(t+=encodeURIComponent(i)+"="+encodeURIComponent(n[i])+"&");t&&(t="#"+t,t=t.slice(0,t.length-1))}return t},h=function(n){typeof console!="undefined"&&console.log&&console.log("[PDFObject] "+n)},n=function(n){return h(n),!1},l=function(n){var t=document.body;return typeof n=="string"?t=document.querySelector(n):typeof jQuery!="undefined"&&n instanceof jQuery&&n.length?t=n.get(0):typeof n.nodeType!="undefined"&&n.nodeType===1&&(t=n),t},f=function(n,t,i,u,f){var e=u+"?file="+encodeURIComponent(t)+i,o=r?"-webkit-overflow-scrolling: touch; overflow-y: scroll; ":"overflow: hidden; ",s="<div style='"+o+"position: absolute; top: 0; right: 0; bottom: 0; left: 0;'><iframe  "+f+" src='"+e+"' style='border: none; width: 100%; height: 100%;' frameborder='0'><\/iframe><\/div>";return n.className+=" pdfobject-container",n.style.position="relative",n.style.overflow="auto",n.innerHTML=s,n.getElementsByTagName("iframe")[0]},a=function(n,t,i,r,u,f,e){var o="";return o=t&&t!==document.body?"width: "+u+"; height: "+f+";":"position: absolute; top: 0; right: 0; bottom: 0; left: 0; width: 100%; height: 100%;",n.className+=" pdfobject-container",n.innerHTML="<embed "+e+" class='pdfobject' src='"+i+r+"' type='application/pdf' style='overflow: auto; "+o+"'/>",n.getElementsByTagName("embed")[0]},c=function(t,u,e){if(typeof t!="string")return n("URL is not valid");u=typeof u!="undefined"?u:!1;e=typeof e!="undefined"?e:{};var v=e.id&&typeof e.id=="string"?"id='"+e.id+"'":"",w=e.page?e.page:!1,b=e.pdfOpenParams?e.pdfOpenParams:{},y=typeof e.fallbackLink!="undefined"?e.fallbackLink:!0,d=e.width?e.width:"100%",g=e.height?e.height:"100%",nt=typeof e.assumptionMode=="boolean"?e.assumptionMode:!0,tt=typeof e.forcePDFJS=="boolean"?e.forcePDFJS:!1,h=e.PDFJS_URL?e.PDFJS_URL:!1,o=l(u),k="",c="";return o?(w&&(b.page=w),c=s(b),tt&&h?f(o,t,c,h,v):i||nt&&p&&!r?a(o,u,t,c,d,g,v):h?f(o,t,c,h,v):(y&&(k=typeof y=="string"?y:"<p>This browser does not support inline PDFs. Please download the PDF to view it: <a href='[url]'>Download PDF<\/a><\/p>",o.innerHTML=k.replace(/\[url\]/g,t)),n("This browser does not support embedded PDFs"))):n("Target element cannot be determined")},{embed:function(n,t,i){return c(n,t,i)},pdfobjectversion:function(){return v}(),supportsPDFs:function(){return i}()}});