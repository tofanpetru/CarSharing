import React, { Component } from 'react';
import {NavMenu}  from './Navbar/NavMenu';

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <div>
        <NavMenu />
        <div>
          {this.props.children}
        </div>
      </div>
    );
  }
}
