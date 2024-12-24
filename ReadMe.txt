[V] Add EFModels

會員系統
[V] Add 註冊新會員
	.add /Models/Infra/HashUtility class
	.在 web.config 中加入 appsettings,key="Salt"
	.add /Models/ViewModels/RegisterVm class
	.add RegisterConfirm view page

[Working On] 實作會員確認功能
	.add /Members/ActiveRegister, url是 /Members/ActiveRegister/?memberId=P&confirmCode=xxx
	.add ActiveRegister view page
[] 實作發送 Email 功能
[] 註冊成功後。寄送Email