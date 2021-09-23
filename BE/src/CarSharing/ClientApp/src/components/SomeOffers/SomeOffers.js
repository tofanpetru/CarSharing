import React, { Component } from "react";
import './SomeOffers.scss';
import Audi from '../Images/Audi.png';
import Bmw from '../Images/Bmw.png';
import Vector from '../Images/Vector.png';


import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";


export default class SomeOffers extends Component {
    render() {

        return (
            <section className="some-offers">
                <h2> Some offers </h2>

                <div className="offers-container">
                    <div className="car-container">
                        <div className="image-container">
                        <img src={ Audi }/>
                        </div>
                        <p className="car-title">Audi Q4 2017</p>
                        <div className="caracteristics">
                            <div >
                                <p>Automatic</p>
                                <p>140 000 km</p>
                            </div>
                            <div>
                                <p>Black</p>
                                <p>5 seats</p>
                            </div>
                        </div>
                        <p className="price">
                            45 € per day
                        </p>
                        <div className="price-button">
                            <button type="button">Reserve now <img src={Vector} /></button>
                        </div>
                    </div>

                    <div className="car-container">
                        <div className="image-container">
                            <img src={Bmw} />
                        </div>
                        <p className="car-title">Audi Q4 2017</p>
                        <div className="caracteristics">
                            <div >
                                <p>Automatic</p>
                                <p>140 000 km</p>
                            </div>
                            <div>
                                <p>Black</p>
                                <p>5 seats</p>
                            </div>
                        </div>
                        <p className="price">
                            45 € per day
                        </p>
                        <div className="price-button">
                            <button type="button">Reserve now <img src={Vector} /></button>
                        </div>
                    </div>

                    <div className="car-container">
                        <div className="image-container">
                            <img src={Audi} />
                        </div>
                        <p className="car-title">Audi Q4 2017</p>
                        <div className="caracteristics">
                            <div >
                                <p>Automatic</p>
                                <p>140 000 km</p>
                            </div>
                            <div>
                                <p>Black</p>
                                <p>5 seats</p>
                            </div>
                        </div>
                        <p className="price">
                            45 € per day
                        </p>
                        <div className="price-button">
                            <button type="button">Reserve now <img src={Vector} /></button>
                        </div>
                    </div>
                </div>
            </section>
        );
    }
}