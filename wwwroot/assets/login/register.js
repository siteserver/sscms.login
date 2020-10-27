var $url = '/login/register';

var data = utils.init({
  redirectUrl: decodeURIComponent(utils.getQueryString('redirectUrl')),
  pageType: null,
  isUserRegistrationGroup: null,
  isHomeAgreement: null,
  homeAgreementHtml: null,
  styles: null,
  groups: null,
  isAgreement: false,
  captchaToken: null,
  captchaUrl: null,
  captchaValue: null,
  form: null
});

var methods = {
  insertText: function(attributeName, no, text) {
    var count = this.form[utils.getCountName(attributeName)];
    if (count < no) {
      this.form[utils.getCountName(attributeName)] = no;
    }
    this.form[utils.getExtendName(attributeName, no)] = text;
    this.form = _.assign({}, this.form);
  },

  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.form = {
        captcha: '',
        groupId: 0
      };
      $this.isUserRegistrationGroup = res.isUserRegistrationGroup;
      $this.isHomeAgreement = res.isHomeAgreement;
      $this.homeAgreementHtml = res.homeAgreementHtml;
      $this.styles = res.styles;
      for (var i = 0; i < res.styles.length; i++) {
        var style = res.styles[i];
        $this.form[_.lowerFirst(style.attributeName)] = style.defaultValue;
      }
      $this.form = _.assign({}, $this.form);
      $this.groups = res.groups;

      $this.apiCaptchaReload();
      $this.pageType = 'form';
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnExtendAddClick: function(style) {
    var no = this.form[utils.getCountName(style.attributeName)] + 1;
    this.form[utils.getCountName(style.attributeName)] = no;
    this.form[utils.getExtendName(style.attributeName, no)] = '';
  },

  btnExtendRemoveClick: function(style) {
    var no = this.form[utils.getCountName(style.attributeName)] - 1;
    this.form[utils.getCountName(style.attributeName)] = no;
    this.form[utils.getExtendName(style.attributeName, no)] = '';
  },

  btnPreviewClick: function(attributeName, n) {
    var imageUrl = n ? this.form[utils.getExtendName(attributeName, n)] : this.form[attributeName];
    window.open(imageUrl);
  },

  apiCaptchaReload: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post('/v1/captcha').then(function (response) {
      var res = response.data;

      $this.captchaValue = res.value;
      $this.captchaUrl = $apiUrl + '/v1/captcha/' + res.value;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiCheckCaptcha: function () {
    var $this = this;

    $api.post('/v1/captcha/actions/check', {
      captcha: this.form.captcha,
      value: this.captchaValue
    }).then(function (res) {
      $this.apiSubmit();
    })
    .catch(function (error) {
      utils.error(error);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);

    var payload = _.assign({}, this.form);
    delete payload.captcha;
    delete payload.confirmPassword;

    $api.post($url, payload).then(function (response) {
      var res = response.data;

      if (res.value) {
        $this.successMessage = "恭喜，账号注册成功";
      } else {
        $this.successMessage = "账号注册成功，请等待管理员审核";
      }
      $this.pageType = 'success';
      if ($this.redirectUrl) {
        setTimeout(function () {
          top.location.href = $this.redirectUrl;
        }, 1500);
      }
    }).catch(function (error) {
      $this.apiCaptchaReload();
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnRegisterClick: function () {
    var $this = this;

    this.$refs.form.validate(function(valid) {
      if (valid) {
        $this.apiCheckCaptcha();
      }
    });
  },

  btnLoginClick: function() {
    location.href = utils.getRootUrl('login');
  },

  validatePass: function(rule, value, callback) {
    if (value === '') {
      callback(new Error('请再次输入密码'));
    } else if (value !== this.form.password) {
      callback(new Error('两次输入密码不一致!'));
    } else {
      callback();
    }
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});