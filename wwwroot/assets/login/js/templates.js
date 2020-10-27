var $url = '/login/templates';

var data = utils.init({
  type: utils.getQueryString('type'),
  templateInfoList: null,
  name: null,
  templateHtml: null,
});

var methods = {
  getIconUrl: function (templateInfo) {
    return '/assets/login/templates/' + templateInfo.name + '/' + templateInfo.icon;
  },

  getCode: function (templateInfo) {
    return '<stl:login type="' + templateInfo.name + '"></stl:login>';
  },

  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: {
        type: this.type
      }
    }).then(function (response) {
      var res = response.data;

      $this.templateInfoList = res.templateInfoList;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnEditClick: function (name) {
    var url = utils.getRootUrl('login/templatesLayerEdit', {
      type: this.type,
      name: name
    });
    utils.openLayer({
      title: '模板设置',
      url: url
    });
  },

  btnHtmlClick: function (templateInfo) {
    var url = utils.getRootUrl('login/templateHtml', {
      type: this.type,
      name: templateInfo.name
    });
    utils.addTab('代码编辑', url);
  },

  btnDeleteClick: function (template) {
    var $this = this;
    utils.alertDelete({
      title: '删除模板',
      text: '此操作将删除模板' + template.name + '，确认吗？',
      callback: function () {
        utils.loading(true);
        $api.delete($url, {
          data: {
            type: $this.type,
            name: template.name
          }
        }).then(function (response) {
          var res = response.data;

          $this.templateInfoList = res.templateInfoList;
        }).catch(function (error) {
          utils.error(error);
        }).then(function () {
          utils.loading($this, false);
        });
      }
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    utils.loading(true);
    $api.post($url, {
      name: this.name,
      templateHtml: this.templateHtml
    }).then(function (response) {
      var res = response.data;

      utils.success('模板编辑成功！');
      $this.pageType = 'list';
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnNavClick: function() {
    console.log(this.type);
    utils.loading(true);
    location.href = utils.getRootUrl('login/templates', {
      type: this.type
    });
  },

  btnPreviewClick: function(template) {
    var url = '/assets/login/templates/' + template.name + '/index.html';
    window.open(url);
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