import React, { Component } from "react";
import Slider from "react-slick";
import './Slider.scss';
import Slider1 from '../Images/Slider1.png';


import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";


export default class SimpleSlider extends Component {
    render() {
        const settings = {
            infinite: true,
            speed: 500,
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: true,
        };
        return (
            <section className="Hello there">
                <Slider {...settings}>
                    <div>
                        <div className="image-container">
                            <img src={Slider1} alt="slider image"/>
                            <div className="centered-text">
                                <p>Reserve now</p>
                            </div>
                            <div className="centered-button">
                                <button type="button"> View cars</button>
                            </div>
                        </div>
                    </div>
                    <div>
                        <div className="image-container">
                            <img src={Slider1} alt="slider image"/>
                            <div className="centered-text">
                                <p>Reserve now</p>
                            </div>
                            <div className="centered-button">
                                <button type="button"> View cars</button>
                            </div>
                        </div>
                    </div>
                </Slider>
            </section>
        );
    }
}