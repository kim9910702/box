import{c as g,r as u,w as R,f as Q,h as a,T as j}from"./index.0f68fa61.js";import{Q as I}from"./QSpinner.3a3a96a0.js";import{c as $,h as E}from"./render.14eca4fd.js";const F={ratio:[String,Number]};function H(e,r){return g(()=>{const o=Number(e.ratio||(r!==void 0?r.value:void 0));return isNaN(o)!==!0&&o>0?{paddingBottom:`${100/o}%`}:null})}const L=16/9;var U=$({name:"QImg",props:{...F,src:String,srcset:String,sizes:String,alt:String,crossorigin:String,decoding:String,referrerpolicy:String,draggable:Boolean,loading:{type:String,default:"lazy"},fetchpriority:{type:String,default:"auto"},width:String,height:String,initialRatio:{type:[Number,String],default:L},placeholderSrc:String,fit:{type:String,default:"cover"},position:{type:String,default:"50% 50%"},imgClass:String,imgStyle:Object,noSpinner:Boolean,noNativeMenu:Boolean,noTransition:Boolean,spinnerColor:String,spinnerSize:String},emits:["load","error"],setup(e,{slots:r,emit:o}){const v=u(e.initialRatio),h=H(e,v);let t;const n=[u(null),u(m())],l=u(0),s=u(!1),c=u(!1),q=g(()=>`q-img q-img--${e.noNativeMenu===!0?"no-":""}menu`),w=g(()=>({width:e.width,height:e.height})),C=g(()=>`q-img__image ${e.imgClass!==void 0?e.imgClass+" ":""}q-img__image--with${e.noTransition===!0?"out":""}-transition`),T=g(()=>({...e.imgStyle,objectFit:e.fit,objectPosition:e.position}));R(()=>S(),y);function S(){return e.src||e.srcset||e.sizes?{src:e.src,srcset:e.srcset,sizes:e.sizes}:null}function m(){return e.placeholderSrc!==void 0?{src:e.placeholderSrc}:null}function y(i){clearTimeout(t),c.value=!1,i===null?(s.value=!1,n[l.value^1].value=m()):s.value=!0,n[l.value].value=i}function z({target:i}){t!==null&&(clearTimeout(t),v.value=i.naturalHeight===0?.5:i.naturalWidth/i.naturalHeight,_(i,1))}function _(i,d){t===null||d===1e3||(i.complete===!0?N(i):t=setTimeout(()=>{_(i,d+1)},50))}function N(i){t!==null&&(l.value=l.value^1,n[l.value].value=null,s.value=!1,c.value=!1,o("load",i.currentSrc||i.src))}function k(i){clearTimeout(t),s.value=!1,c.value=!0,n[l.value].value=null,n[l.value^1].value=m(),o("error",i)}function b(i){const d=n[i].value,f={key:"img_"+i,class:C.value,style:T.value,crossorigin:e.crossorigin,decoding:e.decoding,referrerpolicy:e.referrerpolicy,height:e.height,width:e.width,loading:e.loading,fetchpriority:e.fetchpriority,"aria-hidden":"true",draggable:e.draggable,...d};return l.value===i?(f.class+=" q-img__image--waiting",Object.assign(f,{onLoad:z,onError:k})):f.class+=" q-img__image--loaded",a("div",{class:"q-img__container absolute-full",key:"img"+i},a("img",f))}function B(){return s.value!==!0?a("div",{key:"content",class:"q-img__content absolute-full q-anchor--skip"},E(r[c.value===!0?"error":"default"])):a("div",{key:"loading",class:"q-img__loading absolute-full flex flex-center"},r.loading!==void 0?r.loading():e.noSpinner===!0?void 0:[a(I,{color:e.spinnerColor,size:e.spinnerSize})])}return y(S()),Q(()=>{clearTimeout(t),t=null}),()=>{const i=[];return h.value!==null&&i.push(a("div",{key:"filler",style:h.value})),c.value!==!0&&(n[0].value!==null&&i.push(b(0)),n[1].value!==null&&i.push(b(1))),i.push(a(j,{name:"q-transition--fade"},B)),a("div",{class:q.value,style:w.value,role:"img","aria-label":e.alt},i)}}});export{U as Q};
