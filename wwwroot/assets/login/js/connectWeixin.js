var $url = "/login/connectWeixin"

var data = utils.init({
  form: {
    isWeixin: false,
    weixinAppId: '',
    weixinAppSecret: ''
  },
  url: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.form.isWeixin = res.settings.isWeixin;
      $this.form.weixinAppId = res.settings.weixinAppId;
      $this.form.weixinAppSecret = res.settings.weixinAppSecret;

      $this.url = res.url;
    }).catch(function (error) {
      utils.error($this, error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(true);
    $api.post($url, this.form).then(function (response) {
      var res = response.data;

      $this.url = res.value;
      $this.$message.success('设置保存成功');
    }).catch(function (error) {
      utils.error($this, error);
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
  },

  btnTestClick: function () {
    window.open(this.url);
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
