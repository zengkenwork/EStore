[V] Add EFModels

會員系統
[V] Add 註冊新會員
	.add /Models/Infra/HashUtility class
	.在 web.config 中加入 appsettings,key="Salt"
	.add /Models/ViewModels/RegisterVm class
	.add RegisterConfirm view page

[V] 實作會員確認功能
	.add /Members/ActiveRegister, url是 /Members/ActiveRegister/?memberId=P&confirmCode=xxx
	.add ActiveRegister view page, 範本: empty

[] 實作登入/登出功能
	-只有帳密正確且 IsCofirm = true 的會員才許登入, 請事先準備已/未開通帳戶以便測試
	.在web.config System.Web 中加入 authentication name->cookie名稱 loginUrl->p1到p2時發現沒有登入就進入此url(return p2) defaultUrl->若直接進入登入頁登入完return此url
	.add /Models/ViewModels/LoginVm class
	.modify MembersController, add Login action
	.add Login wiew page, 範本: create
	.暫時做一個簡單的會員中心頁 /Members/Index
	.modify MembersController, add Logout action
	.modify _Layout.cshtml, add 登入/登出的連結(沒登入與已登入,要顯示不同連結)
	.修改 Home/About action,將它設為必須登入才能檢視, 以便確認登入功能是否正常

[] 實作發送 Email 功能
[] 註冊成功後。寄送Email