var $url = '/login/login';

var data = utils.init({
  redirectUrl: decodeURIComponent(utils.getQueryString('redirectUrl')),
  pageType: '',
  captcha: '',
  captchaValue: '',
  captchaUrl: null,
  user: null,
  form: {
    account: '',
    password: '',
    captcha: ''
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
        $this.apiCaptchaLoad();
      }
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

  apiCaptchaLoad: function () {
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

  apiCaptchaCheck: function () {
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

    utils.loading(true);
    $api.post($url, _.assign({}, this.form)).then(function (response) {
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
        $this.apiCaptchaCheck();
      }
    });
  },

  btnLayerClick: function(options) {
    var query = {
      siteId: this.siteId,
      attributeName: options.attributeName
    };
    if (options.no) {
      query.no = options.no;
    }

    var args = {
      title: options.title,
      url: utils.getCommonUrl(options.name, query)
    };
    if (!options.full) {
      args.width = options.width ? options.width : 700;
      args.height = options.height ? options.height : 500;
    }
    utils.openLayer(args);
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});