Ext.define('LoanAgentExt.view.admin.Admin', {
    extend: 'Ext.tab.Panel',
    xtype: 'app-admin',

    requires: [
        'Ext.plugin.Viewport',
        'Ext.window.MessageBox',

        'LoanAgentExt.view.admin.AdminController',
        'LoanAgentExt.view.admin.AdminModel',
        'LoanAgentExt.view.admin.LoansGrid'
    ],

    controller: 'admin',
    viewModel: 'admin',

    ui: 'navigation',

    tabBarHeaderPosition: 1,
    titleRotation: 0,
    tabRotation: 0,

    header: {
        layout: {
            align: 'stretchmax'
        },
        title: {
            bind: {
                text: '{name}'
            },
            flex: 0
        },
        iconCls: 'fa-th-list'
    },

    tabBar: {
        flex: 1,
        layout: {
            align: 'stretch',
            overflowHandler: 'none'
        }
    },

    responsiveConfig: {
        tall: {
            headerPosition: 'top'
        },
        wide: {
            headerPosition: 'left'
        }
    },

    defaults: {
        bodyPadding: 20,
        tabConfig: {
            responsiveConfig: {
                wide: {
                    iconAlign: 'left',
                    textAlign: 'left'
                },
                tall: {
                    iconAlign: 'top',
                    textAlign: 'center',
                    width: 120
                }
            }
        }
    },

    dockedItems: [{
        xtype: 'toolbar',
        dock: 'top',
        items: [
            '->',
            {
                xtype: 'button',
                text: 'Logout',
                handler: function () {
                    this.up('panel').fireEvent('logout');
                }
            }
        ]
    }],

    items: [{
        title: 'Home',
        iconCls: 'fa-home',
        items: [{
            xtype: 'adminloansgrid'
        }]
    }],

    listeners: {
        logout: function () {
            this.logoutUser();
        }
    },

    logoutUser: function () {
        localStorage.removeItem('accessToken');

        Ext.Msg.alert('Logout', 'You have been logged out.');

        LoanAgentExt.app.setMainView('LoanAgentExt.view.auth.Login');
    }
});