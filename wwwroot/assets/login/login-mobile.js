var $url = '/login/loginMobile';
var $urlSendSms = '/login/loginMobile/actions/sendSms';

var data = utils.init({
  redirectUrl: decodeURIComponent(utils.getQueryString('redirectUrl')),
  pageType: '',
  countdown: 0,
  user: null,
  form: {
    mobile: null,
    code: null,
  },
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      if (res.user && res.token) {
        $this.loginSuccess(res.user, res.token, false);
      } else {
        $this.pageType = 'form';
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSendSms: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlSendSms, {
      mobile: this.form.mobile
    }).then(function (response) {
      var res = response.data;

      utils.success('验证码发送成功，10分钟内有效');
      $this.countdown = 60;
      var interval = setInterval(function () {
        $this.countdown -= 1;
        if ($this.countdown <= 0){
          clearInterval(interval);
        }
      }, 1000);
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  loginSuccess: function (user, token, redirect) {
    var $this = this;
    this.user = user;

    localStorage.removeItem(ACCESS_TOKEN_NAME);
    localStorage.setItem(ACCESS_TOKEN_NAME, token);
    this.pageType = 'success';
    if (redirect && this.redirectUrl) {
      setTimeout(function () {
        top.location.href = $this.redirectUrl;
      }, 1500);
    }
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(true);
    $api.post($url, {
      mobile: this.form.mobile,
      code: this.form.code
    }).then(function (response) {
      var res = response.data;
      
      $this.loginSuccess(res.user, res.token, true);
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function(valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});