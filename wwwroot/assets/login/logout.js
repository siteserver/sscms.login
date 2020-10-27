var data = utils.init({
  redirectUrl: decodeURIComponent(utils.getQueryString('redirectUrl')),
  pageType: ''
});

var methods = {
  logout: function () {
    var $this = this;

    localStorage.removeItem(ACCESS_TOKEN_NAME);
    this.pageType = 'success';
    if (this.redirectUrl) {
      setTimeout(function () {
        top.location.href = $this.redirectUrl;
      }, 1500);
    }
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.logout();
  }
});