Ext.define('LoanAgentExt.view.main.Main', {
    extend: 'Ext.tab.Panel',
    xtype: 'app-main',

    requires: [
        'Ext.plugin.Viewport',
        'Ext.window.MessageBox',

        'LoanAgentExt.view.main.MainController',
        'LoanAgentExt.view.main.MainModel',
        'LoanAgentExt.view.main.LoansGrid',
        'LoanAgentExt.view.main.LoanFormWindow'
    ],

    controller: 'main',
    viewModel: 'main',

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
            {
                xtype: 'button',
                text: 'Create Loan',
                handler: function () {
                    Ext.create('LoanAgentExt.view.main.LoanFormWindow').show();
                }
            },
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
            xtype: 'loansgrid'
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