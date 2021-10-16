import React, { Component } from "react";
import './Services.scss';
import Wallet from '../Images/Wallet.png';
import Loop from '../Images/Loop.png';
import Clean from '../Images/Clean.png';




export default class Services extends Component {
    render() {

        return (
            <div className="services-container">
                <div className="first-row">
                    <div className="service">
                        <div className="service-image">
                            <img src={Loop} alt="Loop"/>
                        </div>

                        <div className="service-text">
                            <h2>No hidden fees</h2>
                            <p>Know exactly what you’re paying</p>
                        </div>
                    </div>

                    <div className="service">
                        <div className="service-image">
                            <img src={Wallet} alt="Loop" />
                        </div>

                        <div className="service-text">
                            <h2>Price Match Guarantee</h2>
                            <p>Found the same deal for less? We’ll match the price.</p>
                        </div>
                    </div>
                </div>

                <div className="second-row">
                    <div className="service">
                        <div className="service-image">
                            <img src={Clean} alt="Loop" />
                        </div>

                        <div className="service-text">
                            <h2>Clean cars</h2>
                            <p>We’re working  to keep you safe in the driving seat.</p>
                        </div>
                    </div>
                </div>
                <div className="price-button">
                    <button type="button">Book it now</button>
                </div>
            </div>
        );
    }
}