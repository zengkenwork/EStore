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

[V] 實作登入/登出功能
	-只有帳密正確且 IsCofirm = true 的會員才許登入, 請事先準備已/未開通帳戶以便測試
	.在web.config System.Web 中加入 authentication name->cookie名稱 loginUrl->p1到p2時發現沒有登入就進入此url(return p2) defaultUrl->若直接進入登入頁登入完return此url
	.add /Models/ViewModels/LoginVm class
	.modify MembersController, add Login action
	.add Login wiew page, 範本: create
	.暫時做一個簡單的會員中心頁 /Members/Index
	.modify MembersController, add Logout action
	.modify _Layout.cshtml, add 登入/登出的連結(沒登入與已登入,要顯示不同連結)
	.修改 Home/About action,將它設為必須登入才能檢視, 以便確認登入功能是否正常

[V] 實作修改個資
	.在 /Members/Index 加入修改個資的連結
	.add /Models/ViewModels/ProfileVm class
	.add /Members/Profile action
		-網址不要帶memberId, 從User.Identity.Name取得
	.add Profile view page, 範本: edit

[V] 實作變更密碼
	.在 /Members/Index {partial view} 加入變更密碼的連結
	.add /Models/ViewModels/ChangePasswordVm class
	.add /Members/ChangePassword action, 加入[Authorize]
	.add ChangePassword view page, 範本: create
	.為 ChangePassword view page 加入 partial view

[V] 實作忘記密碼/重設密碼
	.add /Models/ViewModels/ForgotPasswordVm class ,包括 account, email
	.add /Members/ForgotPassword action
	.add ForgotPassword view page, 範本: create
		-比對 account, email, 若正確就 update confirmCode = guid, 並寄送Email
	.postback,成功後, return View("ConfirmForgotPassword")
	.add /Models/ViewModels/ResetPasswordVm class, 包括 password,confirmPassword
	.add /Members/ResetPassword action, url = /Members/ReserPasword/?memberId=99&confirmCode=xxx
	.add ResetPassword view page, 範本: create
		-判斷 memberId, confirmCode 是否正確, 若正確就 update password, confirmCode = null
	.修改 login view page, 加入 '忘記密碼' 的連結

[V] 三層式架構
==============================================================
購物系統
[V] 建立商品清單頁
	.add /Models/ViewModels/ProductIndexVm class
	.add ProductController, add Index action
	.add Index view page, 範本: list, 用來顯示商品清單
	-修改 _Layout.cshtml, 加入nav item

[V] 加入購物車
	.修改 Index.html
	.add CartController, add AddItem(int productId) action

[Working On] 顯示購物車資訊, 實作增減數量的功能
	.modify CartController, addInfo action 顯示購物車明細
	.add UpdateItem(productId, newQty) action
	.在_layout 加入'購物車'連結

[] 實作發送 Email 功能
[] 註冊成功後。寄送Email