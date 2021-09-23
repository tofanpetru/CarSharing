import React, { Component } from 'react';
import './Home.scss';
import SimpleSlider from '../../components/Slider/Slider';
import SomeOffers from '../../components/SomeOffers/SomeOffers';
import Steps from '../../components/Steps/Steps';
import Footer from '../../components/Footer/Footer';

export class Home extends Component {
  static displayName = Home.name;

  render () {
      return (
          <div className="slider-section">
              <SimpleSlider />
              < SomeOffers />
              <Steps/>
               <Footer />
        </div>
    );
  }
}
