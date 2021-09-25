import React, { Component } from "react";
import './Footer.scss';
import { Link } from 'react-router-dom';

import Insta from '../Images/Insta.png';
import Face from '../Images/Face.png';
import Logo from '../Images/Logo.png';

export default class Footer extends Component {
    render() {

        return (
            <section className="footer-container">
                <div className="logo">
                    <img src={Logo} link to="/" alt="footer logo"/>
                </div>

                <div className="contacts">
                    <h2> Contacts </h2>
                    <p><a href="mailto:CarShare@gmail.com">CarShare@gmail.com</a></p>
                    <p><a href="tel:079122189">079122189</a></p>
                   <p>str. Studenților 12/9</p>
                </div>

                <div className="social">
                    <h2>Social media</h2>
                    <img src={Insta} alt="Instagram logo"/>
                    <img src={Face} alt="Facebook logo"/>
                </div>
            </section>
        );
    }
}